import { Button } from 'antd';
import { FC, useState } from 'react';
import { GoogleOutlined } from '@ant-design/icons';
import { SizeType } from 'antd/lib/config-provider/SizeContext';

export const GoogleButton = () => {
  const [size, setSize] = useState<SizeType>('middle');
  return (
    <Button
      type='primary'
      icon={<GoogleOutlined />}
      size={size}
      href='https://localhost:49394/oauth/google'
      style={{ width: '100%' }}
    >
      Google
    </Button>
  );
};
