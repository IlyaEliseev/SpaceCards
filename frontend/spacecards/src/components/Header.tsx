import React, { useEffect, useState } from 'react';
import { Layout } from 'antd';
import Logo from './Logo';
import SignInButton from './SignInButton';
import GetRandomCardButton from './GetRandomCardButton';
const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTk3MTU3OTUsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.5SIsfJCcwYpByVLOoRqmQtDK64FKRqMVr6zPb37suuo';
const { Header } = Layout;

function HeaderComponent() {
  return (
    <Header className='header'>
      <div className='flexContainerHeader'>
        <Logo />
        <GetRandomCardButton />
        <SignInButton />
      </div>
    </Header>
  );
}

export default HeaderComponent;
