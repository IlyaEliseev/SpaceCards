import React, { useState } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { GoogleOutlined } from '@ant-design/icons';

function GoogleAuthButton() {
  const [size, setSize] = useState<SizeType>('middle');

  return (
    <Button
      type='primary'
      icon={<GoogleOutlined />}
      size={size}
      style={{ left: 500 }}
      href='https://google.com'
    >
      Sign in with Google
    </Button>
  );
}

export default GoogleAuthButton;
