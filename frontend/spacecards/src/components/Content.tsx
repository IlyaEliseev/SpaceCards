import React from 'react';
import { Breadcrumb, Layout, Menu } from 'antd';

const { Content } = Layout;

function ContentComponent() {
  return (
    <Layout style={{ padding: '0 24px 24px' }}>
      <Breadcrumb style={{ margin: '14px 0' }}></Breadcrumb>
      <Content
        className='site-layout-background'
        style={{
          padding: 24,
          margin: 0,
          minHeight: 280,
        }}
      ></Content>
    </Layout>
  );
}

export default ContentComponent;
