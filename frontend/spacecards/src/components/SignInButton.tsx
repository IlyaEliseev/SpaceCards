import React, { useState } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { GoogleOutlined, ImportOutlined } from '@ant-design/icons';

function SignInButton(props: { showModal: () => void }) {
  const [size, setSize] = useState<SizeType>('middle');

  return (
    <Button
      icon={<ImportOutlined rotate={180} />}
      size={size}
      href="/signin"
    >
      Sign in
    </Button>
  );
}

export default SignInButton;
