import React, { useState, useEffect } from 'react';
import { Layout, Menu } from 'antd';
import type { MenuProps } from 'antd';
import ContentComponent from './Content';
import AddGroupButton from './AddGroupButton';
import DeleteGroupButton from './DeleteGroupButton';
const { Sider } = Layout;

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTg2NjU0NTcsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.EVFZppOc2sjh57w4d2MlWI3ECzWCbEof-03n0xUT0ko';
const group = { name: '34' };
// const firstGroup = { id: 0, name: 'Cards' };

function Sidebar(props: {
  count: number;
  groupId: number;
  groups: never[];
  setCount: React.Dispatch<React.SetStateAction<number>>;
  setGroupId: React.Dispatch<React.SetStateAction<number>>;
}) {
  // const [cards, setCards] = useState([]);
  // const [groups, setGroups] = useState([]);

  const count = props.count;
  const setCount = props.setCount;
  // const [count, setCount] = useState(0);

  // useEffect(() => {
  //   const fetchGroups = async () => {
  //     const data = await fetch('https://localhost:49394/groups', {
  //       method: 'get',
  //       headers: new Headers({
  //         'Content-type': 'application/json',
  //         Authorization: `Bearer ${token}`,
  //       }),
  //     });
  //     const groups = await data.json();
  //     groups.unshift(firstGroup);
  //     setGroups(groups);
  //   };
  //   fetchGroups().catch(console.error);
  //   console.log(groups);
  // }, [count]);

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

  const items1: MenuProps['items'] = props.groups.map(
    (group: { id: number; name: string }, index) => {
      return {
        id: group.id,
        key: `${group.id}`,
        label: `${group.name}`,
      };
    }
  );

  const getCardsByGroupId = async (groupId: number) => {
    if (groupId > 0) {
      const data = await fetch(`https://localhost:49394/groups/${groupId}`, {
        method: 'get',
        headers: new Headers({
          'Content-type': 'application/json',
          Authorization: `Bearer ${token}`,
        }),
      });
      const group = await data.json();
      const cardsByGroup = group.cards;
    }
  };

  return (
    <>
      <Sider width={200} className='site-layout-background'>
        <div className='flexContainerSidebar'>
          <Menu
            mode='inline'
            defaultSelectedKeys={['1']}
            defaultOpenKeys={['sub']}
            style={{ height: '100%', borderRight: 0 }}
            items={items1}
            onClick={(e) => {
              props.setGroupId(Number(e.key));
              // console.log(`${props.groupId}`);
              // console.log(`${Number(e.key)}`);
            }}
          />
          <AddGroupButton createGroup={createGroup} />
          <DeleteGroupButton id={props.groupId} deleteGroup={deleteGroup} />
        </div>
      </Sider>
    </>
  );
}

export default Sidebar;
