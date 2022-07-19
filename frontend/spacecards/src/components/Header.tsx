import React from 'react';
import { Layout } from 'antd';
import GoogleAuthButton from './GoogleAuthButton';
import GithubAuthButton from './GithubAuthButton';
import Logo from './Logo';

const { Header } = Layout;

function HeaderComponent() {
  return (
    <Header className='header'>
      <div className='flexContainerHeader'>
        <Logo />
        <GoogleAuthButton />
        <GithubAuthButton />
      </div>
    </Header>
  );
}

export default HeaderComponent;
