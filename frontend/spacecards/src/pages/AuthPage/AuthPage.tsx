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
    const response = await fetch(
      `https://localhost:49394/usersaccount/registration`,
      {
        method: 'post',
        mode: 'cors',
        credentials: 'include',
        headers: new Headers({
          'Content-type': 'application/json',
        }),
        body: JSON.stringify(registraionData),
      }
    );
    if (response.ok) {
      await login({
        email: registraionData.email,
        password: registraionData.password,
      });

      const token = sessionStorage.getItem('authtokensuser');
      if (token !== null) {
        navigate('/');
      }
    }
  };

  const login = async (loginData: LoginData) => {
    const response = await fetch('https://localhost:49394/usersaccount/login', {
      method: 'post',
      mode: 'cors',
      credentials: 'include',
      headers: new Headers({
        accept: 'application/json, text/plain, */*',
        'Content-type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      }),
      body: JSON.stringify(loginData),
    });
    if (response.ok) {
      navigate('/');
    }
  };

  const refreshAccessToken = async (refreshTokenData: RefreshTokenData) => {
    const response = await fetch(
      'https://localhost:49394/usersaccount/refreshaccesstoken',
      {
        method: 'post',
        mode: 'cors',
        credentials: 'include',
        headers: new Headers({ 'Content-type': 'application/json' }),
        body: JSON.stringify(refreshTokenData),
      }
    );
    const tokens: Token = await response.json();
    const jsonToken = JSON.stringify(tokens);
    sessionStorage.setItem('authtokensuser', jsonToken);
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
