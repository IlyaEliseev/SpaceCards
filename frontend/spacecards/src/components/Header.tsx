import React, { useEffect, useState } from 'react';
import { Avatar, Layout } from 'antd';
import Logo from './Logo';
import LoginButton from './LoginButton';
import Nickname from './Nickname';
import LogoutButton from './LogoutButton';
const { Header } = Layout;

const findCookieValue = (cookieName: string): string | null => {
  const result =
    document.cookie
      .split('; ')
      .find((row) => row.startsWith(`${cookieName}=`))
      ?.split('=')[1] ?? null;
  return result;
};

const isCookieExist = (cookieName: string): boolean => {
  const result =
    document.cookie
      .split('; ')
      .find((row) => row.startsWith(`${cookieName}=`))
      ?.split('=')[0] ?? null;
  return result === null ? false : true;
};

function HeaderComponent() {
  const nickname: string | null = findCookieValue('nickname');
  const avatarUrl: string | null = findCookieValue('avatar');
  const isAuth: boolean = isCookieExist('session_id');
  const [isAuthentificated, SetIsAuthentificated] = useState<boolean>(isAuth);

  const handleLogoutClick = (): void => {
    SetIsAuthentificated(!isAuthentificated);
  };

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
        <Avatar src={decodeURIComponent(avatarUrl!)} />
        <Nickname nickname={decodeURI(nickname!)} />
        <LogoutButton handleLogoutClick={handleLogoutClick} />
      </Header>
    );
  };

  return isAuthentificated === false ? (
    <>{renderLogin()}</>
  ) : (
    <>{renderLogout()}</>
  );
}

export default HeaderComponent;
