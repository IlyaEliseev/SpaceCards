import React from 'react';
import { ContainerOutlined } from '@ant-design/icons';

function Logo() {
  return (
    <div className='push-left'>
      <div className='logo' />
      <ContainerOutlined
        style={{ fontSize: '35px', color: '#fff', left: '100' }}
      />
    </div>
  );
}

export default Logo;
