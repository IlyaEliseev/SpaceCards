import React, { useEffect, useState } from 'react';
import { Layout } from 'antd';
import Logo from './Logo';
import SignInButton from './SignInButton';
import GetRandomCardButton from './GetRandomCardButton';
import StatisticsButton from './StatisticsButton';
const token = sessionStorage.getItem('refreshtoken');
// ('eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NjIzODY0MzUsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.CUbDEJylJxi6-m5qOfCEtqYS6SEI5mP1DOAH1GtNT_k');
const { Header } = Layout;

function HeaderComponent() {
  return (
    <Header className='header'>
      <div className='flexContainerHeader'>
        <Logo />
        <SignInButton />
      </div>
    </Header>
  );
}

export default HeaderComponent;
