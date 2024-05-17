import { FC, useEffect, useState } from 'react';
import './styles.scss';
import { logService } from '../../services/logService';

interface LogViewProps {
  className: string;
  selectedLog: string | undefined;
}

const LogView: FC<LogViewProps> = ({ className, selectedLog }) => {
  const [log, setLog] = useState<string>();

  const getLog = () => {
    if (selectedLog != undefined) {
      logService.getLogAsync(selectedLog).then((data: string) => {
        setLog(data);
      });
    }
  };

  useEffect(() => {
    getLog();
  }, [selectedLog]);

  useEffect(() => {
    const intervalID = setInterval(() => {
      getLog();
    }, 3000);

    return () => clearInterval(intervalID);
  }, [selectedLog]);

  return (
    <div className={`log-view ${className}`}>
      <div className="log-view__title">
        {selectedLog ? `Содержание файла ${selectedLog}` : 'Файл не выбран'}
      </div>
      <div className="log-view__text-list">
        <div className="log-view__text">{log}</div>
      </div>
    </div>
  );
};

export default LogView;
