import { Breadcrumb } from 'antd';
import React, { FC } from 'react';

interface BreadcrumbComponent {
  pageName: string;
}

export const BreadcrumbComponent: FC<BreadcrumbComponent> = ({ pageName }) => {
  return (
    <Breadcrumb style={{ margin: '16px 0' }}>
      <Breadcrumb.Item href='/'>Home</Breadcrumb.Item>
      <Breadcrumb.Item>{pageName}</Breadcrumb.Item>
    </Breadcrumb>
  );
};
