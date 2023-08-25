import { Button } from 'antd';
import { FC, useState } from 'react';
import { MailOutlined } from '@ant-design/icons';
import { SizeType } from 'antd/lib/config-provider/SizeContext';

export const MailRuButton = () => {
  const [size, setSize] = useState<SizeType>('middle');
  return (
    <Button
      type='primary'
      icon={<MailOutlined />}
      size={size}
      href='https://localhost:49394/oauth/mailru'
      style={{ width: '100%' }}
    >
      MailRu
    </Button>
  );
};
