import React from 'react';
import { Tabs } from 'antd';
import RegistrationForm from './RegistrationForm';
import { LoginData, RegistrationData } from './AuthPage';
import { LoginForm } from './LoginForm/LoginForm';
interface AuthTabsProps {
  registraion: (data: RegistrationData) => void;
  login: (data: LoginData) => void;
}

function AuthTabs(props: AuthTabsProps) {
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
