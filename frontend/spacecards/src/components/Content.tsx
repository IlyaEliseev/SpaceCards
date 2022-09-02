import React, { useState, useEffect } from 'react';
import { Layout } from 'antd';
import CardComponent from './Card';
import CardCreation from './CardCreation';
import GroupCard from './GroupCard';

const { Content } = Layout;

const token = sessionStorage.getItem('refreshtoken');

function ContentComponent(props: {
  groupId: number;
  groups: never[];
  cardsFromGroup: never[];
  cards: never[];
  count: number;
  setCount: React.Dispatch<React.SetStateAction<number>>;
}) {
  // const [cards, setCards] = useState([]);
  const [frontSideState, setFrontSide] = useState('');
  const [backSideState, setBackSide] = useState('');
  const card = { frontSide: frontSideState, backSide: backSideState };
  const groupId = props.groupId;

  const count = props.count;
  const setCount = props.setCount;
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
  // }, [count]);

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
    if (cardId > 0) {
      const data = await fetch(`https://localhost:49394/cards/${cardId}`, {
        method: 'delete',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
        body: JSON.stringify(card),
      });
      setCount(count + 1);
    }
  };

  const updateCard = async (
    cardId: number,
    frontSide: string,
    backSide: string
  ) => {
    if (cardId > 0) {
      const data = await fetch(`https://localhost:49394/cards/${cardId}`, {
        method: 'put',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
        body: JSON.stringify({ frontSide, backSide }),
      });
      setCount(count + 1);
    }
  };

  const getCards = (
    deleteCard: (id: number) => void,
    setFrontSide: React.Dispatch<React.SetStateAction<string>>,
    setBackSide: React.Dispatch<React.SetStateAction<string>>,
    updateCard: (cardId: number, backSide: string, frontSide: string) => void
  ) => {
    const cardList = props.cards.map(
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

  const getCardsFromGroup = () => {
    const cardsFromGroupList = props.cardsFromGroup.map(
      (card: { id: number; frontSide: string; backSide: string }) => {
        return (
          <GroupCard
            key={card.id}
            id={card.id}
            frontSide={card.frontSide}
            backSide={card.backSide}
          />
        );
      }
    );
    return cardsFromGroupList;
  };

  return props.groupId === 0 ? (
    <>
      <div className='flexContainerContent'>
        <Content
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
      </div>
    </>
  ) : (
    <>
      <div className='flexContainerContent'>
        <Content
          style={{
            padding: 24,
            margin: 0,
            minHeight: 280,
          }}
        >
          <div className='flexContainerContent'>{getCardsFromGroup()}</div>
        </Content>
      </div>
    </>
  );
}

export default ContentComponent;
