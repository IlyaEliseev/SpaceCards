import { type } from '@testing-library/user-event/dist/type';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TokenClass } from 'typescript';
import { BreadcrumbComponent } from '../../components/Breadcrumb';
import PageWrapper from '../../components/PageWrapper';
import AuthTabs from './AuthTabs';
export interface Token {
  accessToken: string;
  refreshToken: string;
  nickname: string;
}
export interface RegistrationData {
  nickname: string;
  email: string;
  password: string;
}
export interface LoginData {
  email: string;
  password: string;
}

export interface RefreshTokenData {
  accessToken: string;
  refreshToken: string;
}

function AuthPage() {
  let navigate = useNavigate();

  const registrUser = async (registraionData: RegistrationData) => {
    const data = await fetch(
      `https://localhost:49394/usersaccount/registration`,
      {
        method: 'post',
        headers: new Headers({
          'Content-type': 'application/json',
        }),
        body: JSON.stringify(registraionData),
      }
    );
    console.log(registraionData);

    await login({
      email: registraionData.email,
      password: registraionData.password,
    });

    const token = sessionStorage.getItem('authtokensuser');
    if (token !== null) {
      navigate('/');
    }
  };

  const login = async (loginData: LoginData) => {
    const data = await fetch('https://localhost:49394/usersaccount/login', {
      method: 'post',
      headers: new Headers({
        'Content-type': 'application/json',
      }),
      body: JSON.stringify(loginData),
    });

    const tokens: Token = await data.json();
    const jsonToken = JSON.stringify(tokens);
    sessionStorage.setItem('authtokensuser', jsonToken);

    navigate('/');
  };

  const refreshAccessToken = async (refreshTokenData: RefreshTokenData) => {
    const data = await fetch(
      'https://localhost:49394/usersaccount/refreshaccesstoken',
      {
        method: 'post',
        headers: new Headers({ 'Content-type': 'application/json' }),
        body: JSON.stringify(refreshTokenData),
      }
    );
    const tokens: Token = await data.json();
    const jsonToken = JSON.stringify(tokens);
    sessionStorage.setItem('authtokensuser', jsonToken);

    sessionStorage.setItem(typeof tokens.nickname, tokens.nickname);
    sessionStorage.setItem(typeof tokens.refreshToken, tokens.refreshToken);
  };

  return (
    <div>
      <PageWrapper>
        <BreadcrumbComponent pageName='Login' />
        <div className='registration-page'>
          <AuthTabs registraion={registrUser} login={login} />
        </div>
      </PageWrapper>
    </div>
  );
}

export default AuthPage;
