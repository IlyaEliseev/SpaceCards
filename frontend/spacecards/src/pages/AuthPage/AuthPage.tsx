import { Breadcrumb, Layout } from 'antd';
import ColumnGroup from 'antd/lib/table/ColumnGroup';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
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

  const refreshAccessToken = async () => {};

  return (
    <div>
      <PageWrapper>
        <div className='registration-page'>
          <AuthTabs registraion={registrUser} login={login} />
        </div>
      </PageWrapper>
    </div>
  );
}

export default AuthPage;
