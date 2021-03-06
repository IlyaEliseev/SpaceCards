import React, { useState, useEffect } from 'react';
import { Breadcrumb, Layout, Menu } from 'antd';
import CardComponent from './Card';
import AddCardButton from './AddCardButton';
import DeleteCardButton from './DeleteCardButton';
import CardCreation from './CardCreation';
const { Content } = Layout;

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTg2NjU0NTcsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.EVFZppOc2sjh57w4d2MlWI3ECzWCbEof-03n0xUT0ko';

function ContentComponent(props: {
  groupId: number;
  groups: never[];
  cardsFromGroup: never[];
}) {
  const [cards, setCards] = useState([]);
  const [count, setCount] = useState(0);
  const [frontSideState, setFrontSide] = useState('');
  const [backSideState, setBackSide] = useState('');
  const card = { frontSide: frontSideState, backSide: backSideState };
  const groupId = props.groupId;

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

  const updateCard = async (cardId: number) => {
    const data = await fetch(`https://localhost:49394/cards/${cardId}`, {
      method: 'put',
      headers: new Headers({
        'Content-type': 'application/json',
        Authorization: `Bearer ${token}`,
      }),
      body: JSON.stringify(card),
    });
    setCount(count + 1);
  };

  const getCards = (
    deleteCard: (id: number) => void,
    setFrontSide: React.Dispatch<React.SetStateAction<string>>,
    setBackSide: React.Dispatch<React.SetStateAction<string>>,
    updateCard: (cardId: number) => void
  ) => {
    const cardList = cards.map(
      (card: { id: number; frontSide: string; backSide: string }) => {
        return (
          <CardComponent
            key={card.id}
            id={card.id}
            frontSide={card.frontSide}
            backSide={card.backSide}
            deleteCard={deleteCard}
            setFrontSide={setFrontSide}
            setBackSide={setBackSide}
            updateCard={updateCard}
            groups={props.groups}
          />
        );
      }
    );
    return cardList;
  };

  const getCardsFromGroup = (
    deleteCard: (id: number) => void,
    setFrontSide: React.Dispatch<React.SetStateAction<string>>,
    setBackSide: React.Dispatch<React.SetStateAction<string>>,
    updateCard: (cardId: number) => void
  ) => {
    const cardsFromGroupList = props.cardsFromGroup.map(
      (card: { id: number; frontSide: string; backSide: string }) => {
        return (
          <CardComponent
            key={card.id}
            id={card.id}
            frontSide={card.frontSide}
            backSide={card.backSide}
            deleteCard={deleteCard}
            setFrontSide={setFrontSide}
            setBackSide={setBackSide}
            updateCard={updateCard}
            groups={props.groups}
          />
        );
      }
    );
    return cardsFromGroupList;
  };

  return props.groupId === 0 ? (
    <>
      <Content
        className='site-layout-background'
        style={{
          padding: 24,
          margin: 0,
          minHeight: 280,
        }}
      >
        <div className='flexContainerContent'>
          {getCards(deleteCard, setFrontSide, setBackSide, updateCard)}
          <CardCreation
            setBackSide={setBackSide}
            setFrontSide={setFrontSide}
            createCard={createCard}
          />
        </div>
      </Content>
    </>
  ) : (
    <>
      <Content
        className='site-layout-background'
        style={{
          padding: 24,
          margin: 0,
          minHeight: 280,
        }}
      >
        <div className='flexContainerContent'>
          {getCardsFromGroup(deleteCard, setFrontSide, setBackSide, updateCard)}
        </div>
      </Content>
    </>
  );
}

export default ContentComponent;
