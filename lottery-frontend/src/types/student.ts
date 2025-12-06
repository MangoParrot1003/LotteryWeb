export interface Student {
    id: number
    serialNumber: number
    studentId: string
    name: string
    gender: '女' | '男'
    major: string
    class: string
}

export interface FilterOptions {
    className: string
    gender: string
}

export interface LotteryHistory {
    student: Student
    timestamp: number
}
