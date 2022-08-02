import React, { useEffect, useState } from 'react';
import GuessedCard from './GuessedCard';
import { Layout } from 'antd';
const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTk3MTU3OTUsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.5SIsfJCcwYpByVLOoRqmQtDK64FKRqMVr6zPb37suuo';
const { Content } = Layout;

function GuessingCardPage() {
  const [randomCards, setRandomCards] = useState([]);
  const countRandomCards = 10;

  const [count, setCount] = useState(0);

  const getRandomCardFromGroup = async (countRandomCards: number) => {
    const data = await fetch(
      `https://localhost:49394/groups/randomCards?countCards=${countRandomCards}`,
      {
        method: 'get',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      }
    );
    const cards = await data.json();
    setRandomCards(cards);
    console.log(cards);
  };

  useEffect(() => {
    const getRandomCardFromGroup = async (countRandomCards: number) => {
      const data = await fetch(
        `https://localhost:49394/groups/randomCards?countCards=${countRandomCards}`,
        {
          method: 'get',
          headers: new Headers({
            'Content-type': 'application/json',
            Authorization: `Bearer ${token}`,
          }),
        }
      );
      const cards = await data.json();
      setRandomCards(cards);
      console.log(cards);
    };
    getRandomCardFromGroup(countRandomCards).catch(console.error);
  }, []);

  const getRandomCards = () => {
    const cards = randomCards.map(
      (card: { id: number; frontSide: string; backSide: string }) => {
        return (
          <GuessedCard
            key={card.id}
            id={card.id}
            frontSide={card.frontSide}
            backSide={card.backSide}
            setCount={setCount}
            count={count}
          />
        );
      }
    );
    return cards;
  };

  const te = getRandomCards();

  return <div className='guessingCardForm'>{getRandomCards()[count]}</div>;
}

export default GuessingCardPage;
