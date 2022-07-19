import React, { useState, useEffect } from 'react';
import { Breadcrumb, Layout, Menu } from 'antd';
import CardComponent from './Card';
import AddCardButton from './AddCardButton';
import DeleteCardButton from './DeleteCardButton';
const { Content } = Layout;

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTgzMzUxMjgsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.RCvED_pM7O0NEMVchAxEiNjy_KRwlm5-yApqlYpe--M';
const card = { frontSide: 'Apple', backSide: 'Яблоко' };

function ContentComponent(props: { cardsProps: never[] }) {
  const [cards, setCards] = useState([]);
  const [count, setCount] = useState(0);
  const [cardId, setCardId] = useState(0);
  const [message, setMessage] = useState('');

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

  const createCard = async () => {
    const data = await fetch('https://localhost:49394/cards', {
      method: 'post',
      headers: new Headers({
        'Content-type': 'application/json',
        Authorization: `Bearer ${token}`,
      }),
      body: JSON.stringify(card),
    });
    setCount(count + 1);
  };

  const deleteCard = async (cardId: number) => {
    const data = await fetch(`https://localhost:49394/cards/${cardId}`, {
      method: 'delete',
      headers: new Headers({
        'Content-type': 'application/json',
        Authorization: `Bearer ${token}`,
      }),
      body: JSON.stringify(card),
    });
    console.log(cardId);

    setCount(count + 1);
  };

  const getCards = (deleteCard: (id: number) => void) => {
    const cardList = cards.map(
      (card: { id: number; frontSide: string; backSide: string }) => {
        return (
          <CardComponent
            key={card.id}
            id={card.id}
            frontSide={card.frontSide}
            backSide={card.backSide}
            deleteCard={deleteCard}
          />
        );
      }
    );
    return cardList;
  };

  return (
    <Layout style={{ padding: '0 24px 24px' }}>
      <Breadcrumb style={{ margin: '14px 0' }}></Breadcrumb>
      <Content
        className='site-layout-background'
        style={{
          padding: 24,
          margin: 0,
          minHeight: 280,
        }}
      >
        <div className='flexContainerContent'>
          {getCards(deleteCard)}
          <AddCardButton createCard={createCard} />
        </div>
      </Content>
    </Layout>
  );
}

export default ContentComponent;
