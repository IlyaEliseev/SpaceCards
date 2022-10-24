import { Layout } from 'antd';
import React, { FC, useEffect, useState } from 'react';
import ContentComponent from './Content';
import PageWrapper from '../../components/PageWrapper';
import Sidebar from './Sidebar';
import { Token } from '../AuthPage/AuthPage';

export const MainPage: FC = () => {
  const { Footer } = Layout;
  const [groupId, setGroupId] = useState(0);
  const [cardsFromGroup, setCardsByGroup] = useState([]);
  const [count, setCount] = useState(0);
  const [groups, setGroups] = useState([]);
  const [cards, setCards] = useState([]);

  const authUserInfo: string | null = sessionStorage.getItem('authtokensuser');
  let token: string = '';
  if (authUserInfo !== null) {
    const parseToken: Token = JSON.parse(authUserInfo ?? '');
    token = parseToken.accessToken;
  }

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

  useEffect(() => {
    const fetchGroups = async () => {
      const data = await fetch('https://localhost:49394/groups', {
        method: 'get',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      });
      const groups = await data.json();
      const firstGroup = { id: 0, name: 'Cards' };
      groups.unshift(firstGroup);
      setGroups(groups);
    };
    fetchGroups().catch(console.error);
  }, [count]);

  useEffect(() => {
    if (groupId > 0) {
      const getCardsByGroupId = async (groupId: number) => {
        const data = await fetch(`https://localhost:49394/groups/${groupId}`, {
          method: 'get',
          headers: new Headers({
            'Content-type': 'application/json',
            Authorization: `Bearer ${token}`,
          }),
        });
        const group = await data.json();
        const cardsByGroup = group.cards;
        setCardsByGroup(cardsByGroup);
      };
      getCardsByGroupId(groupId).catch(console.error);
    }
  }, [groupId]);

  return (
    <PageWrapper>
      <Layout className='site-layout-background'>
        <div>
          <Sidebar
            count={count}
            setCount={setCount}
            setGroupId={setGroupId}
            groupId={groupId}
            groups={groups}
          />
        </div>
        <div>
          <ContentComponent
            cards={cards}
            cardsFromGroup={cardsFromGroup}
            groups={groups}
            groupId={groupId}
            count={count}
            setCount={setCount}
          />
        </div>
      </Layout>
      <Footer style={{ textAlign: 'center', minHeight: 590 }}></Footer>
    </PageWrapper>
  );
};
