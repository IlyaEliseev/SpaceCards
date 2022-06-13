import React, { useState } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { GithubOutlined } from '@ant-design/icons';

function GithubAuthButton() {
  const [size, setSize] = useState<SizeType>('middle');

  return (
    <Button
      type='primary'
      icon={<GithubOutlined />}
      size={size}
      href='https://github.com'
    >
      Sign in with Github
    </Button>
  );
}

export default GithubAuthButton;
