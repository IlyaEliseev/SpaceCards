import React from 'react';
import { Tabs } from 'antd';
import LoginForm from './LoginForm';
import RegistrationForm from './RegistrationForm';
import { LoginData, RegistrationData } from './AuthPage';

function AuthTabs(props: {
  registraion: (data: RegistrationData) => void;
  login: (data: LoginData) => void;
}) {
  return (
    <div>
      <Tabs defaultActiveKey='1'>
        <Tabs.TabPane tab='Login' key='1'>
          <LoginForm login={props.login} />
        </Tabs.TabPane>
        <Tabs.TabPane tab='Register' key='2'>
          <RegistrationForm registraion={props.registraion} />
        </Tabs.TabPane>
      </Tabs>
    </div>
  );
}

export default AuthTabs;
