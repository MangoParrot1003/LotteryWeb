import { ref, computed } from 'vue'
import type { Student, FilterOptions, LotteryHistory } from '../types/student'
import studentsData from '../data/students.json'

export function useLottery() {
    const allStudents = ref<Student[]>(studentsData as Student[])
    const filterOptions = ref<FilterOptions>({
        className: '全部',
        gender: '全部'
    })
    const history = ref<LotteryHistory[]>([])
    const currentStudent = ref<Student | null>(null)
    const isAnimating = ref(false)

    // 获取所有班级列表
    const classList = computed(() => {
        const classes = new Set(allStudents.value.map(s => s.class))
        return ['全部', ...Array.from(classes).sort()]
    })

    // 过滤后的学生列表（排除已抽取的）
    const availableStudents = computed(() => {
        const drawnIds = new Set(history.value.map(h => h.student.id))

        return allStudents.value.filter(student => {
            if (drawnIds.has(student.id)) return false

            const classMatch = filterOptions.value.className === '全部' ||
                student.class === filterOptions.value.className
            const genderMatch = filterOptions.value.gender === '全部' ||
                student.gender === filterOptions.value.gender

            return classMatch && genderMatch
        })
    })

    // 随机抽取学生
    const drawStudent = () => {
        if (availableStudents.value.length === 0 || isAnimating.value) return

        isAnimating.value = true
        currentStudent.value = null

        // 预先确定最终要抽取的学生
        const finalIndex = Math.floor(Math.random() * availableStudents.value.length)
        const selectedStudent = availableStudents.value[finalIndex]

        // 类型安全检查
        if (!selectedStudent) {
            isAnimating.value = false
            return
        }

        // 动画效果：快速切换学生
        let count = 0
        const maxCount = 30
        const interval = setInterval(() => {
            count++

            if (count >= maxCount) {
                // 动画最后一帧直接显示最终选中的学生
                clearInterval(interval)
                currentStudent.value = selectedStudent

                // 添加到历史记录
                history.value.unshift({
                    student: selectedStudent,
                    timestamp: Date.now()
                })

                isAnimating.value = false
            } else {
                // 动画过程中随机显示学生
                const randomIndex = Math.floor(Math.random() * availableStudents.value.length)
                currentStudent.value = availableStudents.value[randomIndex] || null
            }
        }, 50)
    }

    // 清空历史记录
    const clearHistory = () => {
        history.value = []
        currentStudent.value = null
    }

    // 统计信息
    const stats = computed(() => {
        const total = allStudents.value.length
        const available = availableStudents.value.length
        const drawn = history.value.length

        const genderStats = allStudents.value.reduce((acc, s) => {
            acc[s.gender] = (acc[s.gender] || 0) + 1
            return acc
        }, {} as Record<string, number>)

        const classStats = allStudents.value.reduce((acc, s) => {
            acc[s.class] = (acc[s.class] || 0) + 1
            return acc
        }, {} as Record<string, number>)

        return {
            total,
            available,
            drawn,
            genderStats,
            classStats
        }
    })

    return {
        filterOptions,
        classList,
        availableStudents,
        currentStudent,
        isAnimating,
        history,
        stats,
        drawStudent,
        clearHistory
    }
}
