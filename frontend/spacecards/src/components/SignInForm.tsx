import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input } from 'antd';
import React, { useState } from 'react';

interface Tokens {
  token: string;
  refreshToken: string;
}

function SignInForm() {
  const [email, setEmail] = useState('');
  const [password, setpassword] = useState('');
  console.log(password);

  const signInData = { email: email, password: password };

  const onFinish = (values: any) => {
    console.log('Received values of form: ', values);
    getTokens();
  };

  const getTokens = async () => {
    const data = await fetch('https://localhost:49394/usersaccount/login', {
      method: 'post',
      headers: new Headers({
        'Content-type': 'application/json',
      }),
      body: JSON.stringify(signInData),
    });
    const tokens: Tokens = await data.json();
    console.log(tokens);
    console.log(tokens.refreshToken);
    sessionStorage.setItem('refreshtoken', tokens.refreshToken);
  };

  return (
    <div>
      <div className='signInForm'>
        <h1 className='stars'>SignIn</h1>
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
              Sign in
            </Button>
            Or <a href='/registration'>register now!</a>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
}

export default SignInForm;
