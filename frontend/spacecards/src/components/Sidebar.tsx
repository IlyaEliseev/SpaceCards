import React, { useState, useEffect } from 'react';
import { EditOutlined } from '@ant-design/icons';
import { Layout, Menu } from 'antd';
import type { MenuProps } from 'antd';
import AddGroupButton from './AddGroupButton';
import DeleteGroupButton from './DeleteGroupButton';
import Input from 'antd/lib/input/Input';
const { Sider } = Layout;

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTk0NTE2MTgsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.MBwc7CYKj79OAUnQutHaO9Ee8CU7--Ya4o43_Z3WXk0';

function Sidebar(props: {
  count: number;
  groupId: number;
  groups: never[];
  setCount: React.Dispatch<React.SetStateAction<number>>;
  setGroupId: React.Dispatch<React.SetStateAction<number>>;
}) {
  const [groupName, setGroupName] = useState('');
  const count = props.count;
  const setCount = props.setCount;

  const createGroup = async () => {
    const data = await fetch('https://localhost:49394/groups', {
      method: 'post',
      headers: new Headers({
        'Content-type': 'application/json',
        Authorization: `Bearer ${token}`,
      }),
      body: JSON.stringify({ name: groupName }),
    });
    setCount(count + 1);
    setGroupName('');
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
      if (index === 0) {
        return {
          id: group.id,
          key: `${group.id}`,
          label: `${group.name}`,
        };
      } else {
        return {
          id: group.id,
          key: `${group.id}`,
          icon: <EditOutlined />,
          label: `${group.name}`,
        };
      }
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
      <div className='flexContainerSidebar'>
        <Sider width={190} className='site-layout-background'>
          <div className='flexContainerSidebar'>
            <Menu
              mode='inline'
              defaultSelectedKeys={['1']}
              defaultOpenKeys={['sub']}
              style={{ height: '100%', borderRight: 0 }}
              items={items1}
              onClick={(e) => {
                props.setGroupId(Number(e.key));
              }}
            />
            <Input
              placeholder='Group name'
              value={groupName}
              onChange={(e) => {
                setGroupName(e.target.value);
              }}
            />
            <AddGroupButton createGroup={createGroup} />
            <DeleteGroupButton id={props.groupId} deleteGroup={deleteGroup} />
          </div>
        </Sider>
      </div>
    </>
  );
}

export default Sidebar;
