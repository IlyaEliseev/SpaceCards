import React, { useEffect, useState } from 'react';
import { Layout } from 'antd';
import Logo from './Logo';
import LoginButton from './LoginButton';
import { Token } from '../pages/AuthPage/AuthPage';
import Nickname from './Nickname';
import LogoutButton from './LogoutButton';
const { Header } = Layout;

function HeaderComponent() {
  const token: string | null = sessionStorage.getItem('authtokensuser');
  let nickname: string = '';
  if (token !== null) {
    const parseToken: Token = JSON.parse(token ?? '');
    nickname = parseToken.nickname;
  }

  const renderLogin = () => {
    return (
      <Header className='headerContent'>
        <Logo />
        <LoginButton />
      </Header>
    );
  };
  const renderLogout = () => {
    return (
      <Header className='headerContent'>
        <Logo />
        <Nickname nickname={nickname} />
        <LogoutButton />
      </Header>
    );
  };

  return token === null ? <>{renderLogin()}</> : <>{renderLogout()}</>;
}

export default HeaderComponent;
