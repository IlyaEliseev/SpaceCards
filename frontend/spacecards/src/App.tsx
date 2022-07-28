import React, { useEffect, useState } from 'react';
import './App.css';
import CardComponent from './components/Card';
import HeaderComponent from './components/Header';
import Sidebar from './components/Sidebar';
import ContentComponent from './components/Content';
import 'antd/dist/antd.min.css';
import { Layout, Modal } from 'antd';
import SignInForm from './components/SignInForm';

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTkxMjQ5ODcsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.WJ24HDscDyC6Ft8qyGC33VZ9g6gFv8WhTEaqOw2lc4w';

const group = { name: 'Brazilian' };
const card = { frontSide: 'Apple', backSide: 'Яблоко' };

function App() {
  const { Header, Content, Footer, Sider } = Layout;
  const [groupId, setGroupId] = useState(0);
  const [cardsFromGroup, setCardsByGroup] = useState([]);
  const [count, setCount] = useState(0);
  const [cards, setCards] = useState([]);
  const [groups, setGroups] = useState([]);

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
    console.log(groupId);
  }, [count]);

  // useEffect(() => {
  //   const fetchGroups = async () => {
  //     const data = await fetch('https://localhost:49394/groups', {
  //       method: 'get',
  //       headers: new Headers({
  //         'Content-type': 'application/json',
  //         Authorization: `Bearer ${token}`,
  //       }),
  //     });
  //     const groups = await data.json();
  //     setGroups(groups);
  //   };
  //   fetchGroups().catch(console.error);
  // }, []);

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
    console.log(cardsFromGroup);
  }, [groupId]);

  // useEffect(() => {
  //   const createGroup = async () => {
  //     const data = await fetch('https://localhost:49394/groups', {
  //       method: 'post',
  //       headers: new Headers({
  //         'Content-type': 'application/json',
  //         Authorization: `Bearer ${token}`,
  //       }),
  //       body: JSON.stringify(group),
  //     });
  //   };
  //   createGroup().catch(console.error);
  // }, []);

  // useEffect(() => {
  //   const createCard = async () => {
  //     const data = await fetch('https://localhost:49394/cards', {
  //       method: 'post',
  //       headers: new Headers({
  //         'Content-type': 'application/json',
  //         Authorization: `Bearer ${token}`,
  //       }),
  //       body: JSON.stringify(card),
  //     });
  //   };
  //   createCard().catch(console.error);
  // }, []);

  return (
    <div className='App'>
      <header>
        <Layout>
          <HeaderComponent />
          <Layout className='site-layout-background'>
            <Sidebar
              count={count}
              setCount={setCount}
              setGroupId={setGroupId}
              groupId={groupId}
              groups={groups}
            />
            <ContentComponent
              cardsFromGroup={cardsFromGroup}
              groups={groups}
              groupId={groupId}
            />
          </Layout>
          <SignInForm />
        </Layout>
      </header>
    </div>
  );
}

export default App;
