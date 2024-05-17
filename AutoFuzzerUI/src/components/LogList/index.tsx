import { ChangeEvent, FC, useEffect, useState } from 'react';
import './styles.scss';
import { logService } from '../../services/logService';

interface LogListProps {
  className: string;
  selectedLog: string | undefined;
  setSelectedLog: (value: string) => void;
}

const LogList: FC<LogListProps> = ({
  className,
  selectedLog,
  setSelectedLog,
}) => {
  const [logNames, setLogNames] = useState<string[]>();

  const getLogNames = () => {
    logService.getLogNamesAsync().then((data: string[]) => {
      if (data != undefined) {
        setLogNames(data);
      }
    });
  };

  useEffect(() => {
    getLogNames();
  }, []);

  useEffect(() => {
    const intervalID = setInterval(() => {
      getLogNames();
    }, 3000);

    return () => clearInterval(intervalID);
  }, []);

  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    setSelectedLog(event.target.value);
  };

  return (
    <>
      {logNames ? (
        <select
          size={2}
          className={`log-list ${className}`}
          value={selectedLog}
          onChange={handleChange}
        >
          {logNames?.map((logName) => (
            <option
              key={logName}
              value={logName}
              className="log-list__log-name"
            >
              {logName}
            </option>
          ))}
        </select>
      ) : (
        <div className={`log-list ${className}`}>Нет логов</div>
      )}
    </>
  );
};

export default LogList;
