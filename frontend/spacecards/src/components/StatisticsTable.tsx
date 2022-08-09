import { Space, Table, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import React from 'react';

interface DataType {
  key: string;
  title: string;
  percentage: string;
  group: string|null
  tags: string[];
}

interface UserStatistics {
    frontSide: string;
    percentage: string;
    groupName: string|null;
  }

function StatisticsTable(props:{ userStatistics: UserStatistics[] }) {
    const columns: ColumnsType<DataType> = [
        {
          title: 'Title',
          dataIndex: 'title',
          key: 'title',
          width: 170,
          align:'center',
          render: text => <a>{text}</a>,
        },
        {
          title: 'Percentage guessing',
          dataIndex: 'percentage',
          key: 'apercentagege',
          width: 100,
          align:'center',
        },
        {
          title: 'Group',
          dataIndex: 'group',
          key: 'group',
          align:'center',
        },
        
      ];

    let data: DataType[] = [];
    props.userStatistics.forEach((value)=>{
        let element = [{
            key: value.frontSide,
            title: value.frontSide,
            percentage: value.percentage,
            group: value.groupName,
            tags: ['nice', 'developer'],
        }]
        data.push(...element);
    })
    return <div className='statisticsTable'><Table size='middle' pagination={{defaultPageSize:15}} columns={columns} dataSource={data} /></div>
}

export default StatisticsTable;