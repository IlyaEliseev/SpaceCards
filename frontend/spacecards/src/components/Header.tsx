import React from 'react';
import { Layout } from 'antd';
import GoogleAuthButton from './GoogleAuthButton';
import GithubAuthButton from './GithubAuthButton';
import { ContainerOutlined } from '@ant-design/icons';

const { Header } = Layout;

function HeaderComponent() {
  return (
    <Header className='header'>
      <div className='flexContainerHeader'>
        <div className='push-left'>
          <div className='logo' />
          <ContainerOutlined
            style={{ fontSize: '35px', color: '#08c', left: '100' }}
          />
        </div>
        <GoogleAuthButton />
        <GithubAuthButton />
      </div>
    </Header>
  );
}

export default HeaderComponent;
