import React, { useState, useEffect } from 'react';
import { Layout, Menu } from 'antd';
import type { MenuProps } from 'antd';
import ContentComponent from './Content';
import AddGroupButton from './AddGroupButton';
import DeleteGroupButton from './DeleteGroupButton';
const { Sider } = Layout;

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTgzMzUxMjgsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.RCvED_pM7O0NEMVchAxEiNjy_KRwlm5-yApqlYpe--M';
const group = { name: '99' };

function Sidebar(props: { groupsProps: never[]; cardsProps: never[] }) {
  // const groups = props.groupsProps;
  // const cards = props.cardsProps;
  const [cards, setCards] = useState([]);
  const [groups, setGroups] = useState([]);

  const [count, setCount] = useState(0);

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
      setGroups(groups);
    };
    fetchGroups().catch(console.error);
    console.log(groups);
  }, [count]);

  const createGroup = async () => {
    const data = await fetch('https://localhost:49394/groups', {
      method: 'post',
      headers: new Headers({
        'Content-type': 'application/json',
        Authorization: `Bearer ${token}`,
      }),
      body: JSON.stringify(group),
    });
    setCount(count + 1);
  };

  const deleteGroup = async (groupId: number) => {
    const data = await fetch(`https://localhost:49394/groups/${groupId}`, {
      method: 'delete',
      headers: new Headers({
        'Content-type': 'application/json',
        Authorization: `Bearer ${token}`,
      }),
    });
    const groupId1 = await data.json();
    setCount(count - 1);
  };

  const [groupId, setGroupId] = useState(0);

  const items1: MenuProps['items'] = groups.map(
    (group: { id: number; name: string }, index) => {
      return {
        id: group.id,
        key: `${group.id}`,
        label: `${group.name}`,
      };
    }
  );

  const items2: MenuProps['items'] = [1].map((icon, index) => {
    const key = String(index + 1);

    return {
      key: `sub${key}`,
      label: `Cards`,
    };
  });

  return (
    <>
      <Sider width={200} className='site-layout-background'>
        <div className='flexContainerSidebar'>
          <Menu
            mode='inline'
            defaultSelectedKeys={['1']}
            defaultOpenKeys={['sub']}
            style={{ height: '100%', borderRight: 0 }}
            items={items2}
          />
          <Menu
            mode='inline'
            defaultSelectedKeys={['1']}
            defaultOpenKeys={['sub']}
            style={{ height: '100%', borderRight: 0 }}
            items={items1}
            onClick={(e) => setGroupId(Number(e.key))}
          />
          <AddGroupButton createGroup={createGroup} />
          <DeleteGroupButton id={groupId} deleteGroup={deleteGroup} />
        </div>
      </Sider>
      <ContentComponent cardsProps={cards} />
    </>
  );
}

export default Sidebar;
