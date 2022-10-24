import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input } from 'antd';
import React, { FC, useState } from 'react';
import { LoginData } from './AuthPage';
interface LoginFormProps {
  login: (data: LoginData) => void;
}

export const LoginForm: FC<LoginFormProps> = React.memo(({ login }) => {
  const [email, setEmail] = useState('');
  const [password, setpassword] = useState('');

  const onFinish = () => {
    login({ password: password, email: email });
  };

  return (
    <div>
      <div className='signInForm'>
        <Form
          name='normal_login'
          initialValues={{ remember: true }}
          onFinish={onFinish}
        >
          <Form.Item
            name='email'
            rules={[{ required: true, message: 'Please input your Username!' }]}
          >
            <Input
              className='input'
              prefix={<UserOutlined className='site-form-item-icon' />}
              value={email}
              placeholder='Email'
              onChange={(e) => setEmail(e.target.value)}
            />
          </Form.Item>
          <Form.Item
            name='password'
            rules={[{ required: true, message: 'Please input your Password!' }]}
          >
            <Input
              prefix={<LockOutlined className='site-form-item-icon' />}
              type='password'
              placeholder='Password'
              value={password}
              onChange={(e) => setpassword(e.target.value)}
            />
          </Form.Item>
          <Form.Item>
            <Form.Item name='remember' valuePropName='checked' noStyle>
              <Checkbox>Remember me</Checkbox>
            </Form.Item>

            <a className='login-form-forgot' href=''>
              Forgot password
            </a>
          </Form.Item>

          <Form.Item>
            <Button
              type='primary'
              htmlType='submit'
              className='login-form-button'
            >
              Login
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
});
