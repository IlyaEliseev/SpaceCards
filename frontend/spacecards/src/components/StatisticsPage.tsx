import React, { useEffect, useState } from 'react';
import StatisticsTable from './StatisticsTable';
const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NjIzODY0MzUsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.CUbDEJylJxi6-m5qOfCEtqYS6SEI5mP1DOAH1GtNT_k';

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

function StatisticsPage(props: { cards: never[] }) {
  const [statistics, setStatistics] = useState<Statistics[]>([]);
  let [id, cardId, success] = statistics;
  const cards = props.cards;

  useEffect(() => {
    const fetchStatistics = async () => {
      const data = await fetch('https://localhost:49394/statistics', {
        method: 'get',
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

  return <StatisticsTable userStatistics={userStatistics} />;
}

export default StatisticsPage;
