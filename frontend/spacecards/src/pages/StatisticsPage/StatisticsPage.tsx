import { Breadcrumb } from 'antd';
import React, { useEffect, useState } from 'react';
import { BreadcrumbComponent } from '../../components/Breadcrumb';
import PageWrapper from '../../components/PageWrapper';
import { Token } from '../AuthPage/AuthPage';
import StatisticsTable from './StatisticsTable';
interface StatisticsPageProps {
  cards: never[];
}

interface Statistics {
  id: number;
  cardId: number;
  success: number;
}

interface UserStatistics {
  frontSide: string;
  percentage: string;
  groupName: string | null;
}

function StatisticsPage(props: StatisticsPageProps) {
  const [statistics, setStatistics] = useState<Statistics[]>([]);
  let [id, cardId, success] = statistics;
  const cards = props.cards;

  const authUserInfo: string | null = sessionStorage.getItem('authtokensuser');
  let token: string = '';
  if (authUserInfo !== null) {
    const parseToken: Token = JSON.parse(authUserInfo ?? '');
    token = parseToken.accessToken;
  }

  useEffect(() => {
    const fetchStatistics = async () => {
      const data = await fetch('https://localhost:49394/statistics', {
        method: 'get',
        mode: 'cors',
        credentials: 'include',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      });
      const statistics = await data.json();
      setStatistics(statistics);
    };
    fetchStatistics().catch(console.error);
  }, []);

  const userStatistics: UserStatistics[] = props.cards.map(
    (card: { id: number; frontSide: string }) => {
      let cardId = card.id;
      let frontSide = card.frontSide;
      let success = statistics.reduce(
        (sumValue, currentValue) =>
          currentValue.cardId === card.id
            ? (sumValue + currentValue.success).toString()
            : sumValue,
        ''
      );
      var successlist = success.split('');

      const correctAnswer = (
        value: string,
        index: number,
        thisArray: string[]
      ) => {
        return Number(value) > 0;
      };

      var percentage =
        String(
          (
            (successlist.filter(correctAnswer).length / successlist.length) *
            100
          ).toFixed(0)
        ) + '%';

      let groupName = localStorage.getItem(String(cardId));

      return { frontSide, percentage, groupName };
    }
  );
  console.log(userStatistics);

  return (
    <>
      <PageWrapper>
        <BreadcrumbComponent pageName='Statistics' />
        <div className='statistics'>
          <StatisticsTable userStatistics={userStatistics} />
        </div>
      </PageWrapper>
    </>
  );
}

export default StatisticsPage;
