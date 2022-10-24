import { Layout } from 'antd';
import React from 'react';
import HeaderComponent from './Header';
interface PageWrapperProps {
  children: React.ReactNode;
}

function PageWrapper(props: PageWrapperProps) {
  return (
    <>
      <HeaderComponent />
      {props.children}
    </>
  );
}

export default PageWrapper;
