import React, { useState } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { ImportOutlined } from '@ant-design/icons';

function LoginButton() {
  const [size, setSize] = useState<SizeType>('middle');

  return (
    <Button icon={<ImportOutlined rotate={180} />} size={size} href='/signin'>
      Login
    </Button>
  );
}

export default LoginButton;
