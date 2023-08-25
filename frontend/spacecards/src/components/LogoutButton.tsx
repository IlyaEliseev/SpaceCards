import React, { useState } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { ImportOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

interface LogoutButtonProps {
  handleLogoutClick: () => void;
}

function LogoutButton(props: LogoutButtonProps) {
  const [size, setSize] = useState<SizeType>('middle');
  let navigate = useNavigate();
  const logout = async () => {
    const response = await fetch(
      'https://localhost:49394/usersaccount/logout',
      {
        method: 'post',
        mode: 'cors',
        credentials: 'include',
        headers: new Headers({
          accept: 'application/json, text/plain, */*',
          'Content-type': 'application/json',
          'Access-Control-Allow-Origin': '*',
        }),
      }
    );
    if (response.ok) {
      window.location.reload();
    }
  };

  return (
    <Button
      icon={<ImportOutlined />}
      size={size}
      onClick={async () => {
        await logout();
        props.handleLogoutClick();
      }}
    >
      Logout
    </Button>
  );
}

export default LogoutButton;
