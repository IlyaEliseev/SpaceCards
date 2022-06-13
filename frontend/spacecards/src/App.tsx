import React, { useEffect, useState } from 'react';
import './App.css';
import CardComponent from './components/Card';
import HeaderComponent from './components/Header';
import Sidebar from './components/Sidebar';
import ContentComponent from './components/Content';
import 'antd/dist/antd.min.css';
import { Layout } from 'antd';
import { Collection } from 'typescript';

function App() {
  useEffect(() => {
    const fetchCards = async () => {
      const data = await fetch('https://localhost:49394/cards');
      const cards = await data.json();
      setCards(cards);
    };
    fetchCards().catch(console.error);
  }, []);

  useEffect(() => {
    const fetchGroups = async () => {
      const data = await fetch('https://localhost:49394/groups');
      const groups = await data.json();
      setGroups(groups);
    };
    fetchGroups().catch(console.error);
  }, []);

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
            <Sidebar />
            <ContentComponent />
          </Layout>
        </Layout>

        {/* <div className='site-card-border-less-wrapper'>{getCards()}</div> */}
      </header>
    </div>
  );
}

export default App;
