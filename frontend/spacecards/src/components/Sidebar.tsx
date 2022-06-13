import React from 'react';
import { Layout, Menu } from 'antd';
import {
  LaptopOutlined,
  NotificationOutlined,
  UserOutlined,
} from '@ant-design/icons';
import type { MenuProps } from 'antd';

const { Sider } = Layout;

const items2: MenuProps['items'] = [1, 2, 3, 4, 5].map((icon, index) => {
  const key = String(index + 1);

  return {
    key: `sub${key}`,
    // icon: React.createElement(icon),
    label: `Cards ${key}`,

    // children: new Array(4).fill(null).map((_, j) => {
    //   const subKey = index * 4 + j + 1;
    //   return {
    //     key: subKey,
    //     label: `option${subKey}`,
    //   };
    // }),
  };
});

function Sidebar() {
  return (
    <Sider width={200} className='site-layout-background'>
      <Menu
        mode='inline'
        defaultSelectedKeys={['1']}
        defaultOpenKeys={['sub']}
        style={{ height: '100%', borderRight: 0 }}
        items={items2}
      />
    </Sider>
  );
}

export default Sidebar;
