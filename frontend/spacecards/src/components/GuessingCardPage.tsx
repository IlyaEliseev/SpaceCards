import React, { useEffect, useState } from 'react';
import GuessedCard from './GuessedCard';
import { Layout } from 'antd';
import GuessingStatistics from './GuessingStatistics';
const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTk3MTU3OTUsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.5SIsfJCcwYpByVLOoRqmQtDK64FKRqMVr6zPb37suuo';
const { Content } = Layout;

function GuessingCardPage() {
  const [randomCards, setRandomCards] = useState([]);
  const countRandomCards = 10;
  const [successValueList, setSuccessValueList] = useState('');
  const [count, setCount] = useState(0);

  const setStatistics = async (cardId: Number, successValue: Number) => {
    const data = await fetch(
      `https://localhost:49394/Statistics/${cardId}?successValue=${successValue}`,
      {
        method: 'post',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      }
    );
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
            setStatistics={setStatistics}
            successValueList={successValueList}
            setSuccessValueList={setSuccessValueList}
          />
        );
      }
    );
    return cards;
  };

  const te = getRandomCards();

  return count < randomCards.length ? (
    <div className='guessingCardForm'>{getRandomCards()[count]}</div>
  ) : (
    <div className='guessingCardForm'>
      <GuessingStatistics successValueList={successValueList} />
    </div>
  );
}

export default GuessingCardPage;
