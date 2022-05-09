void delaySafeMillis(unsigned long timeToWaitMilli) 
{
    unsigned long start_time = millis();
    while (millis() - start_time <= timeToWaitMilli) 
    { 
        /* just hang out but give the uC a chance to do other things too */ 
    }
}

void delaySafeMicros(unsigned long timeToWaitMicro) 
{
    unsigned long start_time = micros();
    while (micros() - start_time <= timeToWaitMicro) 
    { 
        /* just hang out but give the uC a chance to do other things too */ 
    }
}