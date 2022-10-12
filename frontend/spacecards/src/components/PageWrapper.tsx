import { Layout } from 'antd';
import React from 'react';
import HeaderComponent from './Header';

function PageWrapper(props: { children: React.ReactNode }) {
  return (
    <>
      <Layout>
        <HeaderComponent />
        {props.children}
      </Layout>
    </>
  );
}

export default PageWrapper;
