import React from 'react';
import { Layout } from 'antd';
import GoogleAuthButton from './GoogleAuthButton';
import GithubAuthButton from './GithubAuthButton';
import Logo from './Logo';
import SignInButton from './SignInButton';

const { Header } = Layout;

function HeaderComponent(props: { showModal: () => void }) {
  return (
    <Header className='header'>
      <div className='flexContainerHeader'>
        <Logo />
        <SignInButton showModal={props.showModal} />
      </div>
    </Header>
  );
}

export default HeaderComponent;
