import React, { useState } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { ImportOutlined } from '@ant-design/icons';

function LogoutButton() {
  const [size, setSize] = useState<SizeType>('middle');

  const logout = () => {
    sessionStorage.removeItem('authtokensuser');
  };

  return (
    <Button
      icon={<ImportOutlined />}
      size={size}
      onClick={() => {
        logout();
      }}
      href='/'
    >
      Logout
    </Button>
  );
}

export default LogoutButton;
