import React, { useEffect, useState } from 'react';
import './App.css';
import CardComponent from './components/Card';
import HeaderComponent from './components/Header';
import Sidebar from './components/Sidebar';
import ContentComponent from './components/Content';
import 'antd/dist/antd.min.css';
import { Layout } from 'antd';

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTgxMzQzOTgsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0._ALvrg-khrSMihWwDx5JXhZ1bFdtDp64Tz_w4o4SHGA';

const group = { name: 'Brazilian' };
const card = { frontSide: 'Apple', backSide: 'Яблоко' };

function App() {
  // const fetchCards = async () => {
  //   const data = await fetch('https://localhost:49394/cards', {
  //     method: 'get',
  //     headers: new Headers({
  //       'Content-type': 'application/json',
  //       Authorization: `Bearer ${token}`,
  //     }),
  //   });
  //   const cards = await data.json();
  //   setCards(cards);
  // };

  // useEffect(() => {
  //   const fetchCards = async () => {
  //     const data = await fetch('https://localhost:49394/cards', {
  //       method: 'get',
  //       headers: new Headers({
  //         'Content-type': 'application/json',
  //         Authorization: `Bearer ${token}`,
  //       }),
  //     });
  //     const cards = await data.json();
  //     setCards(cards);
  //   };
  //   fetchCards().catch(console.error);
  // }, []);

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

  const getCards = () => {
    const cardList = cards.map(
      (card: { id: number; frontSide: string; backSide: string }) => {
        return (
          <CardComponent
            id={card.id}
            frontSide={card.frontSide}
            backSide={card.backSide}
          />
        );
      }
    );
    return cardList;
  };

  return (
    <div className='App'>
      <header>
        <Layout>
          <HeaderComponent />
          <Layout>
            <Sidebar groupsProps={groups} cardsProps={cards} />
          </Layout>
        </Layout>
      </header>
    </div>
  );
}

export default App;
