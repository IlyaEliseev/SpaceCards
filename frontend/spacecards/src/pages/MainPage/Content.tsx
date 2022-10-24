import React, { useState, useEffect } from 'react';
import { Layout } from 'antd';
import CardComponent from './Card';
import ProdactionCard from './ProdactionCard';
import GroupCard from './GroupCard';
import { Token } from '../AuthPage/AuthPage';

interface ContentComponentProps {
  groupId: number;
  groups: never[];
  cardsFromGroup: never[];
  cards: never[];
  count: number;
  setCount: React.Dispatch<React.SetStateAction<number>>;
}

export interface CardType {
  cardId: number;
  frontSide: string;
  backSide: string;
}
const { Content } = Layout;

function ContentComponent(props: ContentComponentProps) {
  const groupId = props.groupId;

  const count = props.count;
  const setCount = props.setCount;

  const authUserInfo: string | null = sessionStorage.getItem('authtokensuser');
  let token: string = '';
  if (authUserInfo !== null) {
    const parseToken: Token = JSON.parse(authUserInfo ?? '');
    token = parseToken.accessToken;
  }

  const createCard = async (card: CardType) => {
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

  const deleteCard = async (cardId: Number) => {
    if (cardId > 0) {
      const data = await fetch(`https://localhost:49394/cards/${cardId}`, {
        method: 'delete',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      });
      setCount(count + 1);
    }
  };

  const updateCard = async (card: CardType) => {
    const { cardId, frontSide, backSide } = card;
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
    updateCard: (card: CardType) => void
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
            {getCards(deleteCard, updateCard)}
            <ProdactionCard createCard={createCard} />
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
