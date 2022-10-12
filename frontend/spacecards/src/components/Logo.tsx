import React from 'react';
import { ContainerOutlined } from '@ant-design/icons';

function Logo() {
  return (
    <div className='logo'>
      <a href='/'>
        <ContainerOutlined
          style={{ fontSize: '35px', color: '#fff', left: '100' }}
        />
      </a>
    </div>
  );
}

export default Logo;
