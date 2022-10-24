import React, { useEffect, useState } from 'react';
import GuessedCard from './GuessedCard';
import GuessingStatistics from './GuessingStatistics';
import { Token } from '../AuthPage/AuthPage';
import PageWrapper from '../../components/PageWrapper';
import { BreadcrumbComponent } from '../../components/Breadcrumb';

function GuessingCardPage() {
  const [randomCards, setRandomCards] = useState([]);
  const countRandomCards = 10;
  const [successValueList, setSuccessValueList] = useState('');
  const [count, setCount] = useState(0);

  const authUserInfo: string | null = sessionStorage.getItem('authtokensuser');
  let token: string = '';
  if (authUserInfo !== null) {
    const parseToken: Token = JSON.parse(authUserInfo ?? '');
    token = parseToken.accessToken;
  }

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

  return count < randomCards.length ? (
    <PageWrapper>
      <BreadcrumbComponent pageName='Guessed cards' />
      <div className='flexContainerGuessingCard'>
        <div className='guessingCardForm'>{getRandomCards()[count]}</div>
      </div>
    </PageWrapper>
  ) : (
    <PageWrapper>
      <BreadcrumbComponent pageName='Guessed cards' />
      <div className='flexContainerGuessingCard'>
        <GuessingStatistics successValueList={successValueList} />
      </div>
    </PageWrapper>
  );
}

export default GuessingCardPage;
