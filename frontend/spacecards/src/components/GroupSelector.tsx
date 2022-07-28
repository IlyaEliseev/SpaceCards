import { Select } from 'antd';
import { group } from 'console';
import React, { useEffect, useState } from 'react';

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2NTkxMjQ5ODcsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZDRkZGViMzYtYzMyYy00NmZkLThhYTEtZjBhMzFkOWE2YTliIn0.WJ24HDscDyC6Ft8qyGC33VZ9g6gFv8WhTEaqOw2lc4w';

function GroupSelector(props: { groups: never[]; cardId: number }) {
  const { Option } = Select;

  const onChange = (value: string) => {
    setPickGroup(Number(value));
    setGroupName(value);
    console.log(value);
  };

  const onSearch = (value: string) => {
    console.log('search:', value);
  };

  const [pickGroup, setPickGroup] = useState(0);
  const [groupName, setGroupName] = useState('');
  const cardId = props.cardId;
  const groupId = pickGroup;

  useEffect(() => {
    if (cardId > 0 && groupId > 0) {
      const addCardOnGroup = async (cardId: number, groupId: number) => {
        const data = await fetch(
          `https://localhost:49394/groups/${groupId}/cards?cardId=${cardId}`,
          {
            method: 'post',
            headers: new Headers({
              'Content-type': 'application/json',
              Authorization: `Bearer ${token}`,
            }),
          }
        );
      };
      addCardOnGroup(cardId, groupId).catch(console.error);
      console.log('card' + cardId);
    }
  }, [groupId]);

  const selectGroups = () => {
    const selectGroups = props.groups.map(
      (group: { id: number; name: string }) => {
        return (
          <Option key={`${group.id}`} value={`${group.id}`}>
            {group.name}
          </Option>
        );
      }
    );

    return selectGroups;
  };

  return (
    <Select
      showSearch
      placeholder='Select group'
      defaultValue={groupName}
      style={{ width: 120 }}
      optionFilterProp='children'
      onChange={onChange}
      onSearch={onSearch}
      filterOption={(input, option) =>
        (option!.children as unknown as string)
          .toLowerCase()
          .includes(input.toLowerCase())
      }
    >
      {selectGroups()}
    </Select>
  );
}

export default GroupSelector;
