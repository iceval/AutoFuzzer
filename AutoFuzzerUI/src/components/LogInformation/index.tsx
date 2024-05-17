import { FC, useState } from 'react';
import './styles.scss';
import WindowForm from '../WindowForm';
import LogList from '../LogList';
import LogView from '../LogView';

interface LogInformationProps {
  className?: string;
}

const LogInformation: FC<LogInformationProps> = ({ className }) => {
  const [selectedLog, setSelectedLog] = useState<string | undefined>(undefined);

  return (
    <div className={`log-information ${className}`}>
      <WindowForm className="log-information__body" title="Информация о логах">
        <LogList
          className="log-information__list"
          selectedLog={selectedLog}
          setSelectedLog={setSelectedLog}
        />
        <LogView className="log-information__view" selectedLog={selectedLog} />
      </WindowForm>
    </div>
  );
};

export default LogInformation;
