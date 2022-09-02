import React, { useEffect, useState } from 'react';
import './App.css';
import HeaderComponent from './components/Header';
import Sidebar from './components/Sidebar';
import ContentComponent from './components/Content';
import 'antd/dist/antd.min.css';
import { Breadcrumb, Divider, Layout, Modal } from 'antd';
import SignInForm from './components/SignInForm';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import GuessingCardPage from './components/GuessingCardPage';
import StatisticsPage from './components/StatisticsPage';
import RegistrationPage from './components/RegistrationPage';

const token = sessionStorage.getItem('refreshtoken');
interface Cards {
  id: number;
  frontSide: string;
  backSide: string;
}

function App() {
  const { Header, Content, Footer, Sider } = Layout;
  const [groupId, setGroupId] = useState(0);
  const [cardsFromGroup, setCardsByGroup] = useState([]);
  const [count, setCount] = useState(0);
  const [groups, setGroups] = useState([]);
  const [cards, setCards] = useState([]);

  useEffect(() => {
    const fetchCards = async () => {
      const data = await fetch('https://localhost:49394/cards', {
        method: 'get',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      });
      const cards = await data.json();
      setCards(cards);
    };
    fetchCards().catch(console.error);
  }, [count]);

  useEffect(() => {
    const fetchGroups = async () => {
      const data = await fetch('https://localhost:49394/groups', {
        method: 'get',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      });
      const groups = await data.json();
      const firstGroup = { id: 0, name: 'Cards' };
      groups.unshift(firstGroup);
      setGroups(groups);
    };
    fetchGroups().catch(console.error);
  }, [count]);

  useEffect(() => {
    if (groupId > 0) {
      const getCardsByGroupId = async (groupId: number) => {
        const data = await fetch(`https://localhost:49394/groups/${groupId}`, {
          method: 'get',
          headers: new Headers({
            'Content-type': 'application/json',
            Authorization: `Bearer ${token}`,
          }),
        });
        const group = await data.json();
        const cardsByGroup = group.cards;
        setCardsByGroup(cardsByGroup);
      };
      getCardsByGroupId(groupId).catch(console.error);
    }
  }, [groupId]);

  return (
    <Router>
      <Routes>
        <Route
          path='/'
          element={
            <div className='App'>
              <header>
                <Layout>
                  <HeaderComponent />
                  <Layout className='site-layout-background'>
                    <div>
                      <Sidebar
                        count={count}
                        setCount={setCount}
                        setGroupId={setGroupId}
                        groupId={groupId}
                        groups={groups}
                      />
                    </div>
                    <div>
                      <ContentComponent
                        cards={cards}
                        cardsFromGroup={cardsFromGroup}
                        groups={groups}
                        groupId={groupId}
                        count={count}
                        setCount={setCount}
                      />
                    </div>
                  </Layout>
                </Layout>
                <Footer
                  style={{ textAlign: 'center', minHeight: 590 }}
                ></Footer>
              </header>
            </div>
          }
        />
        <Route
          path='/signin'
          element={
            <div>
              <HeaderComponent />
              <Breadcrumb style={{ margin: '16px 0' }}>
                <Breadcrumb.Item href='/'>Home</Breadcrumb.Item>
                <Breadcrumb.Item>SignIn</Breadcrumb.Item>
              </Breadcrumb>
              <div className='flexContainerSigninForm'>
                <SignInForm />
              </div>
            </div>
          }
        />
        <Route
          path='/guessingCards'
          element={
            <div>
              <HeaderComponent />
              <Breadcrumb style={{ margin: '16px 0' }}>
                <Breadcrumb.Item href='/'>Home</Breadcrumb.Item>
                <Breadcrumb.Item>Guessed cards</Breadcrumb.Item>
              </Breadcrumb>
              <div className='flexContainerGuessingCard'>
                <GuessingCardPage />
              </div>
            </div>
          }
        />
        <Route
          path='/Statistics'
          element={
            <div>
              <Breadcrumb style={{ margin: '16px 0' }}>
                <Breadcrumb.Item href='/'>Home</Breadcrumb.Item>
                <Breadcrumb.Item>Statistics</Breadcrumb.Item>
              </Breadcrumb>
              <div className='statistics'>
                <StatisticsPage cards={cards} />
              </div>
            </div>
          }
        />
        <Route
          path='/registration'
          element={
            <div>
              <HeaderComponent />
              <Breadcrumb style={{ margin: '16px 0' }}>
                <Breadcrumb.Item href='/'>Home</Breadcrumb.Item>
                <Breadcrumb.Item>Registration</Breadcrumb.Item>
              </Breadcrumb>
              <div className='flexContainerRegistrationForm'>
                <RegistrationPage />
              </div>
            </div>
          }
        />
      </Routes>
    </Router>
  );
}

export default App;
