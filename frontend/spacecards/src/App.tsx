import React, { useEffect, useState } from 'react';
import './App.css';
import 'antd/dist/antd.min.css';
import { Layout } from 'antd';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import GuessingCardPage from './pages/GuessingPage/GuessingCardPage';
import StatisticsPage from './pages/StatisticsPage/StatisticsPage';
import AuthPage, { Token } from './pages/AuthPage/AuthPage';
import { MainPage } from './pages/MainPage/MainPage';

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

  const authUserInfo: string | null = sessionStorage.getItem('authtokensuser');
  let token: string = '';
  if (authUserInfo !== null) {
    const parseToken: Token = JSON.parse(authUserInfo ?? '');
    token = parseToken.accessToken;
  }

  useEffect(() => {
    const fetchCards = async () => {
      const data = await fetch('https://localhost:49394/cards', {
        method: 'get',
        mode: 'cors',
        credentials: 'include',
        headers: new Headers({
          accept: 'application/json, text/plain, */*',
          'Content-type': 'application/json',
        }),
      });
      console.log(data);
      const cards = await data.json();
      setCards(cards);
    };
    fetchCards().catch(console.error);
  }, [count]);

  useEffect(() => {
    const fetchGroups = async () => {
      const data = await fetch('https://localhost:49394/groups', {
        method: 'get',
        mode: 'cors',
        credentials: 'include',
        headers: new Headers({
          'Content-type': 'application/json',
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
          mode: 'cors',
          credentials: 'include',
          headers: new Headers({
            'Content-type': 'application/json',
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
              <MainPage />
            </div>
          }
        />
        <Route
          path='/signin'
          element={
            <div>
              <AuthPage />
            </div>
          }
        />
        <Route path='/guessingCards' element={<GuessingCardPage />} />
        <Route path='/Statistics' element={<StatisticsPage cards={cards} />} />
      </Routes>
    </Router>
  );
}

export default App;
