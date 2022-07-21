import React, { useEffect, useState } from 'react';
import './App.css';
import CardComponent from './components/Card';
import HeaderComponent from './components/Header';
import Sidebar from './components/Sidebar';
import ContentComponent from './components/Content';
import 'antd/dist/antd.min.css';
import { Layout } from 'antd';

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTg2NjU0NTcsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.EVFZppOc2sjh57w4d2MlWI3ECzWCbEof-03n0xUT0ko';

const group = { name: 'Brazilian' };
const card = { frontSide: 'Apple', backSide: 'Яблоко' };

function App() {
  const [groupId, setGroupId] = useState(0);
  const [cardsByGroup, setCardsByGroup] = useState([]);

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
      setGroups(groups);
    };
    fetchGroups().catch(console.error);
  }, []);

  useEffect(() => {
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
    console.log(cardsByGroup);
  }, []);

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

  const [cards, setCards] = useState([]);
  const [groups, setGroups] = useState([]);

  return (
    <div className='App'>
      <header>
        <Layout>
          <HeaderComponent />
          <Layout>
            <Sidebar
              groupsProps={groups}
              cardsProps={cards}
              setGroupId={setGroupId}
              groupId={groupId}
            />
            <Layout style={{ padding: '0 24px 24px' }}>
              <ContentComponent groupId={groupId} />
            </Layout>
          </Layout>
        </Layout>
      </header>
    </div>
  );
}

export default App;
